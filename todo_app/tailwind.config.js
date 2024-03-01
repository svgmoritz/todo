/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{cshtml, html, razor, js}"],
    theme: {
        extend: {},
    },
    plugins: [require('@tailwindcss/forms')],
}



