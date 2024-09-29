# Home Exchange Matching Algorithms to Reduce Commute Times

This project proposes a system where people living in large cities can reduce their commute times to work or school by exchanging homes with others. The system allows individuals to exchange homes, thus reducing the amount of time spent in traffic. This idea is particularly interesting for **university students** and **apartment residents** in the United States, as it provides a unique solution to the common problem of long commutes.

![Home Exchange Matching](readme-images/cover-image.gif)

The project employs various algorithms to find optimal home exchange matches, including well-known algorithms like **Stable Roommate** and **Top Trading Cycles (TTC)**, as well as the **Preference Rank** algorithm, which I developed specifically for this problem.

---
## Algorithms Used

1. **Irving's Algorithm** (Stable Roommates Problem):
   - Finds stable matches for reciprocal home exchanges between individuals.
   - Based on the participants' mutual preferences.
   - (Irving 1985)

2. **Gale's Top Trading Cycles Algorithm (TTC)** (Housing Market Problem):
   - Used for multi-match scenarios. It creates a cycle based on each participant's preferences, and participants in the cycle exchange homes.
   - (Shapley and Scarf 1974)

3. **Preference Rank Algorithm** (Proposed Algorithm):
   - Matches participants based on their preference rankings, offering a more flexible and optimized solution for home exchanges.

---
## Project Objective

The primary goal of this project is to offer an alternative solution to reduce commute times for people living in high-traffic areas. By allowing individuals to exchange homes, the project explores whether commute times can be significantly reduced.

---
## Stable Roommate Algorithm (Irving's Algorithm)

Irving's Algorithm is used to compute the stable matching for one-sided matching problems such as the Stable Roommates Problem (Irving 1985). The Stable Roommates Problem involves finding a stable matching for an even-sized set. The general idea behind the algorithm is to pair up elements according to their preference lists within the same set. However, the originally proposed version of the algorithm cannot be directly applied to solve the home-exchange matching problem because, in the Stable Roommates Problem, all agents are matched, while in the home-exchange matching problem, only agents that reduce commute times need to be matched. Therefore, the Stable Roommates Algorithm has been adapted to address the home-exchange matching problem.

The Stable Roommates Algorithm can be broken down into three main steps:

1. **Proposal Phase**:
   - In this phase, candidates make proposals to their preferred choices. The first candidate will propose to their most preferred choice. The candidate being proposed to will hold the proposal unless they receive a better offer, in which case they will reject the pending proposal. Those who get rejected will continue proposing until they are accepted. This process continues until all candidates are holding a proposal or one candidate has been rejected by everyone. If any candidate is rejected by all others, then no stable matching exists in the entire system. In this study, if a candidate is rejected by everyone, they are removed from the match pool, allowing a stable matching to be found among the remaining candidates.

2. **Reduction Phase**:
   - In this step, the preference lists are reduced based on the proposals held by each candidate. Since every person holds a unique proposal, anyone still on their preference list who is less preferred than their current proposed partner can be removed. Everyone rejects candidates who are less desirable than their currently accepted proposal, and this process must be done symmetrically.

3. **Cycle Detection Phase**:
   - If there is more than one preference per agent, the algorithm proceeds to the third step, which involves looking for preference cycles. To detect cycles, two arrays, `p` and `q`, are defined. These arrays help spot repetitions that indicate cycles. The process begins by choosing the first candidate from the preference list of more than one person and assigning it as `p0`. Next, `q0` is defined as the second member of `p0`’s preference list. To find `pi+1`, the algorithm locates the last member of `qi`’s preference list. This process continues, and when the value of `p` is repeated, a preference cycle is complete. To eliminate the cycle, `qi` will reject `pi+1` symmetrically. This process is repeated until only one element remains in each preference list. There may be multiple cycles that need to be eliminated to reach this point. If, after removing cycles, any preference list becomes empty, those agents must be removed from the match pool.

In the following sections, the pseudocode for the Stable Roommates Algorithm can be referenced for a clearer understanding of the implementation.

### Pseudocode for Stable Roommates Algorithm

```plaintext
while there are unmatched agents do

	let a1 be first unmatched agent
	a1 proposes to its first preference a2 who has not rejected it previously

	if a2 has not receieved a proposal before then
		a2 accepts a1

	else
		if a2 prefers a1 over its current match a3 then
			a2 accepts a1
			reject symmetrically (a2, a3)
		else
			reject symmetrically (a1, a2)
		end if
	end if
end while

for all a2 holding proposal from a1 do
	reject symmetrically all (a2, a3) where a2 prefers a1 over a3
end for

for all cycles in (p1...pn+1) and (q1...qn) such that:
	qi is the second preference of pi and pi+1 is the last preference of qi do

	for i = 1 .. n do
		rejects symmetrically (q1, pi+1)
	end for
end for
```
