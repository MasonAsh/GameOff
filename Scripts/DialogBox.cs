using Godot;
using System;

public class DialogBox : Node
{
    [Export]
    private NodePath dialogTextPath;
    [Export]
    private NodePath dialogNamePath;

    [Export]
    private float characterProgressTime = 0.05F;

    private RichTextLabel dialogText;
    private RichTextLabel dialogName;

    private String characterName;
    private String[] speech;
    private int speechIndex;

    public override void _Ready()
    {
        dialogText = GetNode(dialogTextPath) as RichTextLabel;
        dialogName = GetNode(dialogNamePath) as RichTextLabel;

        AddUserSignal("dialog_next");
        AddUserSignal("finished");
    }

    public void _StartDialog(String characterName, String[] speech)
    {
        this.characterName = characterName;
        this.speech = speech;
        speechIndex = 0;

        dialogName.SetText(characterName);

        SetProcessInput(true);

        _RunDialog();
    }

    private async void _RunDialog()
    {
        var timer = new Timer();
        AddChild(timer);
        timer.WaitTime = characterProgressTime;
        timer.Start();

        foreach (var part in speech) {
            dialogText.SetText(part);
            dialogText.SetVisibleCharacters(0);

            for (int i = 0; i < part.Length + 1; i++) {
                dialogText.SetVisibleCharacters(i);
                await ToSignal(timer, "timeout");
            }

            await ToSignal(this, "dialog_next");
        }

        RemoveChild(timer);

        EmitSignal("finished");
    }

    public override void _Input(InputEvent ev)
    {
        if (ev is InputEventMouseButton) {
            EmitSignal("dialog_next");
        }
    }
}
