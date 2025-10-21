## Quizz generator with IA

AI Question Generator aimed at individuals seeking to challenge their knowledge and determine their current level of expertise. I conceived this tool to help with my job interviews and as a way to test knowledge. This tool is primarily developed for fun and for practicing one's skills.

### Instructions

- Clone the repository
- Navigate to https://cloud.google.com, **log in** or create an account if you don't have one yet. Then, create a new project.
- Next, navigate to https://aistudio.google.com/welcome and log in. There, you need to generate your **API Key**.
- In your local environment, initialize the secret manager with `dotnet user-secrets init`
- Save your **API Key** using `dotnet user-secrets set "ApiKeys:ApiGen" "YOUR_SECRET_API_KEY"`
