function SubmitRun(ctx: nkruntime.Context, logger: nkruntime.Logger, nk: nkruntime.Nakama, payload: string): string {
  let data = JSON.parse(payload);

  const completionTime = Number(data.completionTime);
  const damageTaken = Number(data.damageTaken);
  const version = data.clientVersion;

  if (completionTime < 5 || completionTime > 300) {
    throw new Error("Invalid completionTimeSec");
  }
  if (damageTaken < 0) {
    throw new Error("Invalid hitsTaken");
  }
  if (version !== "0.1") {
    throw new Error("Invalid client version: " + version);
  }

  if (!ctx.userId || !ctx.username) {
    throw new Error("User tidak valid");
  }
    
  nk.leaderboardRecordWrite(
    "unity_test",
    ctx.userId,
    ctx.username,
    data.completionTime,
    data.damageTaken
  );

  return JSON.stringify({ success: true });
};