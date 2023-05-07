
// testimonials variables
const testimonialsItem = document.querySelectorAll("[data-testimonials-item]");
const modalContainer = document.querySelector("[data-modal-container]");
const modalCloseBttn = document.querySelector("[data-modal-close-bttn]");
const overlay = document.querySelector("[data-overlay]");

// modal variable
const modalImg = document.querySelector("[data-modal-img]");
const modalTitle = document.querySelector("[data-modal-title]");
const modalText = document.querySelector("[data-modal-text]");

// modal toggle function
const testimonialsModalFunc = function () {
modalContainer.classList.toggle("active");
overlay.classList.toggle("active");
}

// add click event to all modal items
for (let i = 0; i < testimonialsItem.length; i++) {

testimonialsItem[i].addEventListener("click", function () {

    modalImg.src = this.querySelector("[data-testimonials-avatar]").src;
    modalImg.alt = this.querySelector("[data-testimonials-avatar]").alt;
    modalTitle.innerHTML = this.querySelector("[data-testimonials-title]").innerHTML;
    modalText.innerHTML = this.querySelector("[data-testimonials-text]").innerHTML;

    testimonialsModalFunc();

});

}

// add click event to modal close button
modalCloseBttn.addEventListener("click", testimonialsModalFunc);
overlay.addEventListener("click", testimonialsModalFunc);

// Получаем элементы кнопок, вызывающих модальные окна
const modalTriggers = document.querySelectorAll('.modal-trigger');

// Привязываем обработчики событий к каждой кнопке
modalTriggers.forEach((trigger) => {
trigger.addEventListener('click', () => {
    // Получаем id модального окна, соответствующего данной кнопке
    const modalId = trigger.getAttribute('data-modal-id');
    const modal = document.getElementById(modalId);

    // Показываем модальное окно
    modal.style.display = 'block';
    });
});

// Закрыть модальное окно при клике на кнопку закрытия
const closeButtons = document.querySelectorAll('.close-modal-change');

closeButtons.forEach((button) => {
button.addEventListener('click', () => {
    const modal = button.closest('.modal-change');
    closeModalChange(modal);
    });
});

// Закрыть модальное окно при клике вне его области
const modals = document.querySelectorAll('.modal-change');

modals.forEach((modal) => {
modal.addEventListener('click', (event) => {
    if (event.target === modal) {
    closeModalChange(modal);
    }
    });
});

// Функция для закрытия модального окна
function closeModalChange(modal) {
modal.style.display = 'none';
}
