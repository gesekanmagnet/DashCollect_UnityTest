let InitModule: nkruntime.InitModule =
        function(ctx: nkruntime.Context, logger: nkruntime.Logger, nk: nkruntime.Nakama, initializer: nkruntime.Initializer) {
	try {
        nk.leaderboardCreate(
        "unity_test",
        true,
        nkruntime.SortOrder.ASCENDING,
        nkruntime.Operator.BEST,
        "0 0 * * 1",
        { description: "" }
        );
        logger.info("Leaderboard created or already exists.");
    } 
    catch (error) {
        logger.error("Failed to create leaderboard: %s", error);
    }
	initializer.registerRpc("SubmitRun", SubmitRun);
    logger.info("Hello World!");
}