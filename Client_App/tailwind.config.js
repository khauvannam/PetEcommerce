/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}", "./node_modules/flowbite/**/*.js"],
    theme: {
        extend: {
            colors: {
                main: "#00205c",
                hover: "#FADC6F"
            },
            fontFamily: {
                heading : ["Recoleta","sans-serif"],
                sans: ['AvenirNext', 'sans-serif']
            }
        },
    },
    plugins: [
        require('flowbite/plugin')
    ],
}

