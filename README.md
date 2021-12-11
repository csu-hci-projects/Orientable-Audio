# Orientable-Audio
This repository holds the final states of the scripts used in the CS 567 project.

The Unity files are stored separately as to not congest this repository. Instead, the simplicity of this repository is meant to easily re-use or re-purpose the existing scripts. If desired to re-use, then supply each of the SerializedFields with the respect audio and game objects to run in a Unity project.

Notably, the GameObject fields in the experiment script are a link list to turn on the next experiment when the current has concluded. Therefore, ONLY KEEP ONE EXPERIMENT GAME OBJECT INSTANCE ACTIVE WHEN STARTING THE UNITY APPLICATION. Otherwise, multiple instances will simulatenous run, which if desired is fine but was not the original use case for this experiment.

Audio sources need to be set to "spatialized" and the 2d-3d slider set to 3d - a value of 1.0. This will ensure the HRTF capabilities are used either within Unity or when deployed to the HoloLens 2.
