export const decimals = (val, n) => {
    val = val ? val : 0;
    return val.toFixed(n);
}
export const formatUnit = (val) => {
    val = val ? Number(val) : 0;
    var str = ''
    if ((val / 100000000) >= 1) {
        str = (val / 100000000).toFixed(2) + 'B'
    } else if ((val / 1000000) >= 1) {
        str = (val / 1000000).toFixed(2) + 'M'
    } else if ((val / 1000) >= 1) {
        str = (val / 1000).toFixed(2) + 'K'
    } else {
        str = val
    }
    // switch (val) {
    //     case (val / 100000000) >= 0:
    //         str = (val / 100000000).toFixed(2) + '亿'
    //         break
    //     case (val / 10000) >= 0:
    //         str = (val / 10000).toFixed(2) + 'b'
    //         break
    //     case (val / 1000) >= 0:
    //         str = (val / 1000).toFixed(2) + 'K'
    //         break
    //     case (val / 1000) < 0:
    //         str = val
    //         break

    // }
    return str;
}
export const dateTiem = (val, n) => {
    val = val ? val : '00:00';
    return val.split(' ')[1].slice(0, 5);
}
export const dateDay = (val, n) => {
    val = val ? val : '';
    return val.split(' ')[0].slice(5, ).replace(/-/g, '/');
}
export const coinSetValue = (val, n) => {
    val = val ? val : '';
    return val.split('#')[n];
}
export const Gender = (val) => {
    let str = ''
        // （0-待到账、1-已到账、2-已退回）
    switch (val) {
        case '1':
            str = '男'
            break
        case '2':
            str = '女'
            break
        case '*':
            str = '未设置'
            break
    }
    return str;
}