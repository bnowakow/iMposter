function spoilerClick(elem) {
    var parentElement = elem.parentNode.parentNode.getElementsByTagName('div')[1].getElementsByTagName('div')[0];
    if (parentElement.style.display != '') {
        parentElement.style.display = '';
        elem.innerText = '';
        elem.value = 'Απόκρυψη';
    }
    else {
        parentElement.style.display = 'none';
        elem.innerText = '';
        elem.value = 'Εμφάνιση';
    }
}