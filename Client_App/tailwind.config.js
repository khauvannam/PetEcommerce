/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}"],
    theme: {
        extend: {
            colors: {
                main: "#00205c",
                hover: "#FADC6F"
            }
        },
    },
    plugins: [],
}

