# ONYX Frontend

## Table of Contents
1. [Technologies](#technologies)
3. [Setup](#setup)
4. [Usage](#usage)
5. [Folder Structure](#folder-structure)
6. [Contributing](#contributing)

## Technologies
List of the major technologies and frameworks used in the project.

- **React**: Version 18.2.0
- **TypeScript**
- **Tailwindcss**
- **Shadcn/ui**
- **tanstack/react-router**
- **tanstack/react-query**
- **zod**
- ...

## Setup
Instructions to set up the project on a local machine.

1. **Clone the repository:**
    ```bash
    git clone https://github.com/dbrdak/onyx-app
    cd <ProjectPath>/onyx-frontend
    ```

2. **Install dependencies:**
    ```bash
    npm install
    ```

## Usage
Instructions on how to run the project.

**Since the frontend part needs an ONYX backend connection to fetch necessary data, and the backend has to be run locally (temporarily), the frontend part contains a server proxy in `vite.config.ts` (to satisfy CORS), configured to run with the backend API opened as a Docker container.**

To set up the backend as Docker container, follow the ONYX-backend README or use the following steps:

#### Download the file for running the API with Docker
[run-script.ps1](https://github.com/DBrdak/onyx-app/blob/penny-migration/onyx-backend/run-script.ps1)

#### Run the file
You'll need to log in to `docker.io` and provide the Azure CosmosDB instance key that has the 'budget' database created.

####Then:

1. **Start the development server:**
    ```bash
    npm start
    ```
   This will run the app in development mode. Open [http://localhost:5173](http://localhost:5173) to view it in the browser.

2. **Build for production:**
    ```bash
    npm run build
    ```
   This will create an optimized production build in the `build` folder.

## Folder Structure
Explanation of the project's folder structure and what each folder/file represents.

```plaintext
onyx-frontend/
├── public
│   └── fonts
├── src/
│   ├── assets/
│   ├── components/
│       ├── ui (reusable components only)
│       └── ...
│   ├── lib/
│   ├── routes/
│   ├── index.css
│   ├── main.tsx
│   └── ...
├── index.html
├── .gitignore
├── package.json
├── vite.config.ts
├── README.md
└── ...
```

## Contributing
Guidelines for contributing to the project.

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch-name`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch-name`).
6. Open a pull request.
