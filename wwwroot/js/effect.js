const onhold = document.querySelector('#onhold');
const comentario = document.querySelector('#comentario');

onhold.addEventListener(
    'click',
    () => {
        if (comentario.style.display === 'none') {
            comentario.style.display = 'block';
        } else {
            comentario.style.display = 'none';
        }
    }
);
