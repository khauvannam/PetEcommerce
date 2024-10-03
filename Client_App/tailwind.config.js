/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{razor,html,cshtml}", "./node_modules/flowbite/**/*.js"],
    theme: {
        extend: {
            colors: {
                main: "#00205c",
                hover: "#FADC6F"
            },
            animation: {
                fadeIn: 'fadeIn 0.5s ease-in forwards',
            },
            keyframes: {
                fadeIn: {
                    '0%': {opacity: '0'},
                    '100%': {opacity: '1'},
                },
            },
            fontFamily: {
                heading: ["Recoleta"],
                sans: ['AvenirNext'],
                heading_bold: ['Recoleta-Bold']
            }
        },
    },
    plugins: [
        require('flowbite/plugin')
    ],
}

