using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using MagicWords.Application.EventChannels;
using MagicWords.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MagicWords.Application.Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEventsChannelSO gameEventsChannel;
        [SerializeField] private UIEventsChannelSO uiEventsChannel;

        private FirebaseAuth auth;
        private FirebaseDatabase database;
        private DatabaseReference databaseReference;
        private DatabaseReference matchReference;

        private string userId;
        private string matchId;

        public string UserId
        {
            get { return userId; }
        }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject); // Opcional: para que el manager persista entre escenas
        }

        private void Start()
        {
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError($"Error initializing Firebase: {task.Exception}");
                    return;
                }

                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                databaseReference = database.RootReference;

                // Autenticación anónima (puedes cambiarlo para usar otros proveedores)
                SignInAnonymously();
            });
        }

        private void SignInAnonymously()
        {
            auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                userId = newUser.UserId;
            });
        }

        public void CreateMatch(string player1Id, GameMode gameMode, string objective, float maxTime, string boardSetup)
        {
            matchId = databaseReference.Child("matches").Push().Key;
            matchReference = databaseReference.Child("matches").Child(matchId);

            Match match = new Match(matchId, player1Id, gameMode, objective, maxTime, boardSetup);
            matchReference.SetRawJsonValueAsync(JsonUtility.ToJson(match));
            // gameEventsChannel.OnMatchCreated(matchId);  //Lo comentamos temporalmente
            // Llamar a una Cloud Function para buscar oponentes (para el modo PvP)
            // ...
        }
        public void StartMatch()
        {
            matchReference.Child("StartTime").SetValueAsync(ServerValue.Timestamp);
        }

        public void EndMatch()
        {
            matchReference.Child("EndTime").SetValueAsync(ServerValue.Timestamp);
        }
        public void SendCellSelected(Cell selectedCell, string playerId)
        {
            if (matchId == null) return;

            // Estructura para enviar datos a Firebase
            var cellData = new Dictionary<string, object>
            {
                { "x", selectedCell.X },
                { "y", selectedCell.Y },
                { "letter", selectedCell.Letter.ToString() },
                { "playerId", playerId }
            };

            // Enviar datos a Firebase
            databaseReference.Child("matches").Child(matchId).Child("moves").Push().SetValueAsync(cellData);
        }

        public void SendWordValidated(List<Cell> word, string playerId, int score)
        {
            if (matchId == null) return;

            // Convertir la lista de celdas a una estructura compatible con Firebase
            var wordData = new List<Dictionary<string, object>>();
            foreach (var cell in word)
            {
                wordData.Add(new Dictionary<string, object>
                {
                    { "x", cell.X },
                    { "y", cell.Y },
                    { "letter", cell.Letter.ToString() }
                });
            }

            // Estructura para enviar datos a Firebase
            var validatedWordData = new Dictionary<string, object>
            {
                { "word", wordData },
                { "playerId", playerId },
                { "score", score }
            };

            // Enviar datos a Firebase
            databaseReference.Child("matches").Child(matchId).Child("validatedWords").Push().SetValueAsync(validatedWordData);
        }

        public async Task CheckPlayerConnection()
        {
            while (true)
            {
                // Envía un ping a la base de datos en tiempo real.
                var pingReference = databaseReference.Child("ping").Child(userId);
                await pingReference.SetValueAsync(ServerValue.Timestamp);

                // Espera un tiempo razonable para la respuesta.
                await Task.Delay(TimeSpan.FromSeconds(5)); // Ajusta el tiempo según sea necesario.

                // Intenta obtener la última marca de tiempo del servidor para este usuario.
                var lastPongSnapshot = await pingReference.GetValueAsync();
                if (lastPongSnapshot.Exists)
                {
                    try
                    {
                        long lastPong = (long)lastPongSnapshot.Value;
                        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        long latency = currentTime - lastPong;

                        // Comprueba si la latencia está dentro de un rango aceptable.
                        if (latency > 5000) // Ajusta el umbral de latencia según sea necesario.
                        {
                            // Latencia alta detectada.
                            uiEventsChannel.RaiseConnectionLost(); // Notifica a la UI sobre la conexión perdida.
                        }
                        else
                        {
                            // Conexión normal.
                            uiEventsChannel.RaiseConnectionRestored(); // Notifica a la UI sobre una conexión estable.
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Error al procesar la respuesta de ping: " + ex.Message);
                        uiEventsChannel.RaiseConnectionLost(); // Considera la conexión como perdida en caso de error.
                    }
                }
                else
                {
                    // No se pudo obtener una respuesta del servidor.
                    uiEventsChannel.RaiseConnectionLost(); // Notifica a la UI sobre la conexión perdida.
                }

                // Espera antes de la próxima verificación.
                await Task.Delay(TimeSpan.FromSeconds(10)); // Ajusta el intervalo según sea necesario.
            }
        }
    }
}