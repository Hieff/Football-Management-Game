// Simulate the match
public string matchSimulation(teamScript homeTeam, teamScript awayTeam){
  int homeScore = 0; int awayScore = 0;

  //Home advantage usually exists
  float homeTeamAdvantage = 1.25f;
  // Bounces included for randomness which occurs
  float luckyBounces = Random.Range(0f,7f); float unluckyBounces = Random.Range(-7f, 0f);
  // More chances based on how much better the team is
  float attackChances = ((homeTeam.getFinalAttack() + luckyBounces + unluckyBounces) * homeTeamAdvantage) / awayTeam.getFinalDefence();
  float remainder = attackChances % 1;
  int whole = Mathf.RoundToInt(attackChances);
  if(whole < 1){whole = 1;}
  float[] numberOfAttacks = new float[whole];
  for(int i = 0; i < whole-1; i++){
    numberOfAttacks[i] = 1;
  }
  numberOfAttacks[whole-1] = remainder;

  // For each chance check if a goal was scored
  for(int i = 0; i < whole; i++){
    float strikersFinish = Random.Range(-7.5f,7.5f);
    // Work out chance to score
    float goalChance = (numberOfAttacks[i] * ((homeTeam.getFinalAttack() + strikersFinish) * homeTeamAdvantage)) / ((awayTeam.getFinalDefence() * 0.125f) + awayTeam.getFinalKeeper());
    // When chance of goal goes over 100, make it high 90s as there is no such thing as a guaranteed goal.
    if(goalChance > 1){
      goalChance = goalChance / (goalChance + 0.1f );
    }

    // Generate a random number. If it is lower than the chance to score then the goal goes in
    goalChance = goalChance * 100;
    int chance = Mathf.RoundToInt(goalChance);
    int randNumber = Random.Range(1, 100);

    if(randNumber < chance){
      homeScore++;
    }
  }
