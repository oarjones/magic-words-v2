import os

# Lista de rutas relativas a la carpeta Assets
folders = [
    "Assets/Art/Sprites",
    "Assets/Art/Textures",
    "Assets/Art/Models",
    "Assets/Art/Animations",
    "Assets/Art/Materials",
    "Assets/Audio/Music",
    "Assets/Audio/SFX",
    "Assets/Fonts",
    "Assets/Prefabs",
    "Assets/Scenes",
    "Assets/Scripts/Domain/Data/Models",
    "Assets/Scripts/Domain/Data/ScriptableObjects",
    "Assets/Scripts/Domain/Logic/Services",
    "Assets/Scripts/Domain/Logic/CoreLogic",
    "Assets/Scripts/Application/Managers",
    "Assets/Scripts/Application/EventChannels",
    "Assets/Scripts/Presentation/UI/Views",
    "Assets/Scripts/Presentation/UI/Presenters",
    "Assets/Tests/EditMode",
    "Assets/Tests/PlayMode",
    "Assets/Resources",
    "Assets/Plugins"
]

def create_folders():
    """
    Crea todas las carpetas definidas en la lista 'folders'.
    """
    for folder in folders:
        os.makedirs(folder, exist_ok=True)
        print(f"Carpeta creada/ya existente: {folder}")

if __name__ == "__main__":
    create_folders()
