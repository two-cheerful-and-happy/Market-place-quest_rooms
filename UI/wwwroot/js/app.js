window.addEventListener('scroll', e => {
    document.body.style.cssText += `--scrollTop: ${this.scrollY}px`
})

gsap.registerPlugin(ScrollTrigger, ScrollSmoother)
ScrollSmoother.create({
    wrapper: '.wrapper',
    content: '.container'
})

// Сценарий, который будет открывать и закрывать модальное окно

//// Получаем элементы DOM, с которыми будем работать
//var modal = document.querySelector(".modal");
//var modalButton = document.querySelector("#modal-button");
//var closeButton = document.querySelector(".close-button");

//// Функция, которая открывает модальное окно
//function openModal() {
//    modal.style.display = "block";
//}

//// Функция, которая закрывает модальное окно
//function closeModal() {
//    modal.style.display = "none";
//}

//// Обработчики событий, которые будут открывать и закрывать модальное окно при нажатии на кнопки
//modalButton.addEventListener("click", openModal);
//closeButton.addEventListener("click", closeModal);

// Получаем элементы DOM, с которыми будем работать
var modal1 = document.getElementById("modal1");
var modalButton1 = document.getElementById("modalButton1");
var closeButton1 = document.getElementById("closeButton1");

var modal2 = document.getElementById("modal2");
var modalButton2 = document.getElementById("modalButton2");
var closeButton2 = document.getElementById("closeButton2");

// Функции, которые открывают и закрывают модальные окна
function openModal1() {
    modal1.style.display = "block";
}

function closeModal1() {
    modal1.style.display = "none";
}

function openModal2() {
    modal2.style.display = "block";
}

function closeModal2() {
    modal2.style.display = "none";
}

// Обработчики событий, которые будут открывать и закрывать модальные окна при нажатии на кнопки
modalButton1.addEventListener("click", openModal1);
closeButton1.addEventListener("click", closeModal1);

modalButton2.addEventListener("click", openModal2);
closeButton2.addEventListener("click", closeModal2);
