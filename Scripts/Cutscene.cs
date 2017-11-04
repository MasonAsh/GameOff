using Godot;
using System;
using System.Collections.Generic;
using Nett;

public class Cutscene : Node
{
    [Export(GD.PROPERTY_HINT_FILE, "*.toml")]
    private String cutsceneFile = null;

    [Export]
    private bool autoStart = true;

    [Export]
    private NodePath dialogBoxPath;

    enum SequenceType {
        Dialog,
        SetBackground,
    }

    class Sequence {
        public SequenceType sequenceType;

        public String dialogCharacterName;
        public String[] dialogSpeech;
    }

    List<Sequence> sequences = new List<Sequence>();


    DialogBox dialogBox;

    public override void _Ready()
    {
        if (cutsceneFile is null) {
            GD.print("No cutscene file specified. Cutscene will not work!!!!");
            return;
        }

        File file = new File();
        file.Open(cutsceneFile, File.READ);

        if (!file.IsOpen()) {
            GD.print("cutscene file set to invalid path! Custcene will not work!!!");
            return;
        }

        dialogBox = (DialogBox)GetNode(dialogBoxPath);

        if (dialogBox is null) {
            GD.print("Bad path for dialog box. CutScene will not work!!!");
            return;
        }

        TomlTable table = Toml.ReadString(file.GetAsText());

        readTOML(table);

        if (autoStart) {
            _Start();
        }
    }

    private void readTOMLDialog(TomlTable sequence) {
        var dialogs = sequence.Get<TomlTableArray>("dialog");
        
        foreach (var dialog in dialogs.Items) {
            var name = dialog.Get<string>("name");
            var speech = dialog.Get<TomlArray>("speech");
            var new_sequence = new Sequence();
            new_sequence.sequenceType = SequenceType.Dialog;
            new_sequence.dialogCharacterName = name;

            String[] strSpeech = new String[speech.Length];

            int i = 0;
            foreach (var speechItem in speech.Items) {
                strSpeech[i] = (speechItem.Get<string>());
                i++;
            }

            new_sequence.dialogSpeech = strSpeech;
            sequences.Add(new_sequence);
        }
    }

    private void readTOML(TomlTable table)
    {
        var sequences = table.Get<TomlTableArray>("sequence");

        foreach (var sequenceValue in sequences.Items)
        {
            var sequence = sequenceValue as TomlTable;

            switch (sequence.Get<string>("type")) {
                case "dialog":
                    readTOMLDialog(sequence);
                    break;
                case "set_background":
                    break;
                default:
                    GD.print("Unrecognized sequence type in cutscene file");
                    break;
            }
        }
    }

    public async void _Start() {
        foreach (var sequence in sequences) {
            if (sequence.sequenceType == SequenceType.Dialog) {
                dialogBox._StartDialog(sequence.dialogCharacterName, sequence.dialogSpeech);

                await ToSignal(dialogBox, "finished");
            }
        }
    }
}
