using Godot;
using System;
using GC= Godot.Collections;
public partial class black_jack : Control
{
	GC.Array Cards = new GC.Array{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10};
	GC.Array DealerHand = new GC.Array();
	GC.Array PlayerHand = new GC.Array();
	int DealerTotal = 0;
	int PlayerTotal = 0;
	Button hitBtn;
	Button standBtn;
	Button restartBtn;
	Label DealerLabel;
	Label PlayerLabel;
	RichTextLabel FinalResult;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetVars();
		ConnectSignals();
		restartBtn.Disabled=true;
		DealCards();
		CheckPlayer();
		GD.Print(PlayerHand);
		GD.Print(DealerHand);
	}

	public void ConnectSignals()
	{
		hitBtn.Connect("pressed", new Callable(this,"HitPressed"));
		standBtn.Connect("pressed", new Callable(this,"StandPressed"));
		restartBtn.Connect("pressed", new Callable(this,"RestartPressed"));

	}

	public void CheckPlayer()
	{
		if (PlayerTotal > 21)
		{
			hitBtn.Disabled = true;
			standBtn.Disabled = true;
			DeclareWinner();
		}
	}

	public void DeclareWinner()
	{
		hitBtn.Disabled = true;
		standBtn.Disabled = true;
		if ((PlayerTotal > DealerTotal && PlayerTotal <=21) || DealerTotal >21 )
		{
			FinalResult.PushBold();
			FinalResult.PushFontSize(80);
			FinalResult.AppendText("[center][color=black]Congrats! You win[/color][/center]");
		}
		else if ((PlayerTotal < DealerTotal && DealerTotal <=21) || PlayerTotal >21 )
		{
			FinalResult.PushBold();
			FinalResult.PushFontSize(80);
			FinalResult.AppendText("[center][color=red]Congrats! You win[/color][/center]");
		}
		else if (PlayerTotal == DealerTotal)
		{
			FinalResult.PushBold();
			FinalResult.PushFontSize(80);
			FinalResult.AppendText("[center][color=blue]Draw! Play Again.[/color][/center]");
		}
		restartBtn.Disabled=false;
	}

	public void HitPressed()
	{
		Hit(PlayerHand);
		PlayerTotal = TotalCards(PlayerHand);
		PlayerLabel.Text = $"Player Total: {PlayerTotal}";
		CheckPlayer();
	}

	public void StandPressed()
	{
		DealerDraw();
		DeclareWinner();
	}

	public void RestartPressed()
	{
		GetTree().ReloadCurrentScene();
	}

	public void DealerDraw() 
	{
		DealerHand.Remove("?");
		Hit(DealerHand);
		DealerTotal = TotalCards(DealerHand);
		while (DealerTotal < 16) {
			Hit(DealerHand);
			DealerTotal = TotalCards(DealerHand);
		}
		DealerLabel.Text = $"Dealer Total: {DealerTotal}";
	}
	public void SetVars()
	{
		hitBtn = GetNode<Button>("hitBtn");
		standBtn = GetNode<Button>("standBtn");
		restartBtn = GetNode<Button>("restartBtn");
		DealerLabel = GetNode<Label>("dealerTotal");
		PlayerLabel = GetNode<Label>("playerTotal");
		FinalResult = GetNode<RichTextLabel>("finalResult");
	}

	public void DealCards()
	{
		Hit(PlayerHand);
		Hit(DealerHand);
		Hit(PlayerHand);
		DealerTotal = TotalCards(DealerHand);
		DealerLabel.Text = $"Dealer Total: {DealerTotal}";
		PlayerTotal = TotalCards(PlayerHand);
		PlayerLabel.Text = $"Player Total: {PlayerTotal}";
		DealerHand.Add("?");
	}

	public int TotalCards(GC.Array hand)
	{
		int Total = 0;
		foreach (int C in hand)
		{
			Total += C;
		}

		return Total;
	}

	public void Hit(GC.Array Hand)
	{
		Hand.Add(Cards.PickRandom());
	}
}
