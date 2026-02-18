/*
//2
function convertStringToNumber (str) {
    return Number.isInteger(str) ? parseInt(str) : false;
}
*/

//4
/*
function createGratitude (name, rating = 0) {
    if (!name)
        name = 'Аноним';

    return `${name} оценил нас на ${rating} из 5. Спасибо, ${name}!`;
}
*/

/*
//5
function checkA1 (a) {
    // 1. if-else
    if (a !== 0)
        return a;
    else
        return 'Все плохо';
}

function checkA2 (a) {
    // 2. тернарный оператор
    return a !== 0 ? a : 'Все плохо';
}

function checkA3 (a) {
    // 3. логическое или
    return a || 'Все плохо';
}
*/

/*
//6
function sumSquares (min, max) {
    let result = 0;
    for (let i = min; i <= max; i++) {
        result += i * i;
    }

    return result;
}
*/

/*
//7
function sumArray (arr) {
    return arr.flat(Infinity).reduce((a, b) => a + b, 0);
}
*/
