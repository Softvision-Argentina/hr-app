export function resizeModal(): void{
    let slider:HTMLElement = document.querySelector('.slick-list');
    let currentSlide:HTMLElement = document.querySelector('.slick-slide.slick-active');
    slider.style.transition = 'height ease-in-out 0.3s';

    setTimeout(()=>{
        slider.style.height = `${currentSlide.offsetHeight}px`
    }, 150)
}