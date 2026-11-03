import os
import librosa
import soundfile as sf

# ERMITTELT DEN AKTUELLEN ORDNER DES SKRIPTS (Egal wo er liegt)
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

# RELATIVE PFADE: Ordner liegen direkt im selben Verzeichnis wie dieses Skript
INPUT_FOLDER = os.path.join(SCRIPT_DIR, "Eingabe")
OUTPUT_FOLDER = os.path.join(SCRIPT_DIR, "Ausgabe")

# Falls die Ordner noch nicht existieren, werden sie automatisch erstellt
os.makedirs(INPUT_FOLDER, exist_ok=True)
os.makedirs(OUTPUT_FOLDER, exist_ok=True)

# HIER IHRE WUNSCH-VARIANTEN DEFINIEREN:
# rate  = Geschwindigkeit (1.2 = 20% schneller, 0.8 = 20% langsamer)
# steps = Tonhöhe in Halbtönen (+2 = zwei Töne höher, -2 = zwei Töne tiefer)
varianten = {
    "schnell_gleicher_pitch": {"rate": 1.2, "steps": 0.0},
    "normal_hoeherer_pitch":  {"rate": 1.0, "steps": 2.0},
    "langsam_tieferer_pitch": {"rate": 0.8, "steps": -2.0}
}

# Alle Audiodateien im Eingabe-Ordner verarbeiten
files = [f for f in os.listdir(INPUT_FOLDER) if f.lower().endswith((".wav", ".mp3", ".flac", ".m4a"))]

if not files:
    print(f"Hinweis: Keine Audiodateien im Ordner '{INPUT_FOLDER}' gefunden.")
    print("Bitte lege zuerst ein paar Audio-Tracks in den 'Eingabe'-Ordner.")

for filename in files:
    file_path = os.path.join(INPUT_FOLDER, filename)
    base_name, _ = os.path.splitext(filename)
    
    print(f"\nVerarbeite Datei: {filename}...")
    
    try:
        # Audio laden
        y, sr = librosa.load(file_path, sr=None)
        
        # Varianten berechnen und exportieren
        for name, settings in varianten.items():
            y_modified = y.copy()
            
            # 1. Geschwindigkeit ändern
            if settings["rate"] != 1.0:
                y_modified = librosa.effects.time_stretch(y_modified, rate=settings["rate"])
                
            # 2. Tonhöhe ändern
            if settings["steps"] != 0.0:
                y_modified = librosa.effects.pitch_shift(y_modified, sr=sr, n_steps=settings["steps"])
            
            # Zielname bauen (z.B. songname_schnell_gleicher_pitch.wav)
            output_name = f"{base_name}_{name}.wav"
            output_path = os.path.join(OUTPUT_FOLDER, output_name)
            
            # Speichern
            sf.write(output_path, y_modified, sr)
            print(f" -> Exportiert: {output_name}")
            
    except Exception as e:
        print(f"Fehler bei Datei {filename}: {e}")

print("\n--- FERTIG! Alle Dateien wurden im 'Ausgabe'-Ordner gespeichert. ---")
