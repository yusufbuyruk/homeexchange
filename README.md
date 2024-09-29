# Home Exchange Matching Algorithms to Reduce Commute Times

This project proposes a system where people living in large cities can reduce their commute times to work or school by exchanging homes with others. The system allows individuals to exchange homes, thus reducing the amount of time spent in traffic. This idea is particularly interesting for **university students** and **apartment residents** in the United States, as it provides a unique solution to the common problem of long commutes.

The project employs various algorithms to find optimal home exchange matches, including well-known algorithms like **Stable Roommate** and **Top Trading Cycles (TTC)**, as well as the **Preference Rank** algorithm, which I developed specifically for this problem.

## Algorithms Used

1. **Stable Roommate Algorithm**:
   - Finds stable matches for reciprocal home exchanges between individuals.
   - Based on the participants' mutual preferences.

2. **Top Trading Cycles (TTC)**:
   - Used for multi-match scenarios. It creates a cycle based on each participant's preferences, and participants in the cycle exchange homes.

3. **Preference Rank Algorithm** (Developed by me):
   - Matches participants based on their preference rankings, offering a more flexible and optimized solution for home exchanges.

## Project Objective

The primary goal of this project is to offer an alternative solution to reduce commute times for people living in high-traffic areas. By allowing individuals to exchange homes, the project explores whether commute times can be significantly reduced.

## Installation

To run the project on your local machine, follow these steps:

1. **Clone the GitHub repository**:
   ```bash
   git clone https://github.com/username/project-name.git
