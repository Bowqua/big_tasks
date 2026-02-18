/*
//1
function divide (a, b) {
    return Math.trunc(a / b);
}
*/

/*
//2
function truncateString (str, len) {
    return str.length > len ? str.substring(0, len) + '…' : str;
}
*/

/*
//3
function palindromeChecker (str) {
    return str.toLowerCase().split('').reverse().join('') === str ? true : false;
}
*/

/*
//4
function formatMoney (money) {
    let fixed = Number(money).toFixed(2);
    let parts = fixed.split('.');

    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ' ');

    return parts.join('.');
}
*/

/*
//5
function getMostFrequentElement (arr) {
    let elements = {};
    let result = null;
    let maxCount = 0;

    for (let number of arr) {
        elements[number] = (elements[number] || 0) + 1;

        if (elements[number] > maxCount) {
            maxCount = elements[number];
            result = number;
        }
    }

    return result;
}
*/

/*
//6
function sample (arr) {
    return arr[Math.floor(Math.random() * arr.length)];
}
*/

/*
//7
function square (arr) {
    return arr.map(a => a ** 2);
}
*/

/*
//8
function sortArr(arr) {
    return [...arr].sort((a, b) => b - a);
}
*/

/*
//9
function calculateAverage() {
    let sum = 0;

    for (let i = 0; i < arguments.length; i++) {
        sum += arguments[i];
    }

    return arguments.length === 0 ? 0 : sum / arguments.length;
}
*/

/*
//10
function formatDate(date) {
    let day = String(date.getDate()).padStart(2, '0');
    let month = String(date.getMonth() + 1).padStart(2, '0');
    let year = String(date.getFullYear());

    return `${day}-${month}-${year}`;
}
*/