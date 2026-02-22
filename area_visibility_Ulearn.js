/*
//3
function createMultiplier(num) {
    let count = 0;
    const a = 8;
    const b = 10;
    const c = 256;

    return function() {
        count++;
        if (count === 1) {
            return num * a;
        }

        else if (count === 2) {
            return num * b;
        }

        else if (count === 3) {
            return num * c;
        }

        count = 1;
        return num * a;
    }
}
*/

/*
//4
function createStorage() {
    let storage = [];
    return {
        add(...numbers) {
            storage.push(...numbers);
        },

        get() {
            return storage;
        },

        clear() {
            storage = [];
        },

        remove(number) {
            storage = storage.filter(digit => digit !== number);
        }
    }
}
*/

/*
//5
function createColor(colorName) {
    return {
        colorName: colorName,
        use() {
            return `Используется ${this.colorName.toLowerCase()} цвет`;
        },

        stopUse() {
            return `${this.colorName[0].toUpperCase() + this.colorName.slice(1).toLowerCase()} цвет больше не используется`
        }
    }
}
*/

/*
//6
function useColor1() {
    // добавить use к объекту newcolor. Нужно переиспользовать use из объекта blueColor.
    const newcolor = {colorName: 'серо-буро-малиновый в крапинку'};
    const blueColor = createColor('Синий');

    newcolor.use = blueColor.use;
    return newcolor.use();
}

function useColor2() {
    // Воспользуйся методом call
    const newcolor = {colorName: 'серо-буро-малиновый в крапинку'};
    const blueColor = createColor('Синий');

    return blueColor.use.call(newcolor);
}

function useColor3() {
    // Воспользуйся методом bind
    const newcolor = {colorName: 'серо-буро-малиновый в крапинку'};
    const blueColor = createColor('Синий');

    return blueColor.use.bind(newcolor)();
}
*/

/*
//7
function delayedHello(name = 'Незнакомец') {
    return setTimeout(() => {
        console.log(`Привет, ${name}!`);
    }, 1000);
}
*/
