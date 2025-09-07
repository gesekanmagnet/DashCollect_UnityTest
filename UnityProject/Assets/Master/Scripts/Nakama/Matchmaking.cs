using UnityEngine;

using Nakama;

public class Matchmaking : MonoBehaviour
{
    private bool isMatched;
    private string matchId;

    private void OnEnable()
    {
        EventCallback.OnLoggedin += LoggedIn;
        EventCallback.OnMatchEnd += MatchEnd;
        EventCallback.OnGameOver += Disconnect;
    }

    private void OnDisable()
    {
        EventCallback.OnLoggedin -= LoggedIn;
        EventCallback.OnMatchEnd -= MatchEnd;
        EventCallback.OnGameOver -= Disconnect;
    }

    private void LoggedIn()
    {
        Connector.socket.ReceivedMatchmakerMatched += OnReceiveMatchmakerMatched;
        Connector.socket.ReceivedMatchState += OnReceiveMatchState;
    }
    
    /// <summary>
    /// Find match through matchmaking
    /// </summary>
    public async void FindMatch()
    {
        await Connector.socket.AddMatchmakerAsync(minCount: 2, maxCount: 2);
        try
        {
            Debug.LogError("Finding");
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }

    /// <summary>
    /// Match finished, sending the winner player session
    /// </summary>
    /// <param name="session">Winner session</param>
    private async void MatchEnd(ISession session)
    {
        if (!isMatched) return;

        await Connector.socket.SendMatchStateAsync(matchId, 1, session.Username);

        EventCallback.OnGameOver(Connector.session.UserId.Equals(session.UserId) ? GameResult.Win : GameResult.Lose);
        Debug.Log(session.Username + ": completed the game");
    }

    private async void OnReceiveMatchmakerMatched(IMatchmakerMatched match)
    {
        EventCallback.OnMatchFound();
        var join = await Connector.socket.JoinMatchAsync(match);
        try
        {
            isMatched = true;
            matchId = match.MatchId;
            Debug.Log(matchId);
            foreach (var item in join.Presences)
            {
                Debug.LogError(item.Username);
                
            }
            EventCallback.OnMatchStart();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }

    private void OnReceiveMatchState(IMatchState matchState)
    {
        if (matchState.OpCode == 1)
        {
            EventCallback.OnGameOver(GameResult.Lose);
        }
    }

    private async void Disconnect(GameResult gameResult)
    {
        if (!isMatched) return;
        await Connector.socket.LeaveMatchAsync(matchId);
    }
}