document.addEventListener('DOMContentLoaded', () => {

    function stylePickrButtons() {
        document.querySelectorAll('.pcr-button').forEach(btn => {
            btn.style.width = '40px';
            btn.style.height = '40px';
        });
    }

    document.querySelectorAll('[id^="pickr_"]').forEach(el => {

        const id = el.id.replace('pickr_', '');
        const input = document.getElementById('value_' + id);

        if (!input || typeof Pickr === 'undefined') return;


        const pickr = Pickr.create({
            el,
            theme: 'classic',
            default: input.value || '#ffffff',
            components: {
                preview: true,
                opacity: false,
                hue: true,
                interaction: {
                    input: true,
                    save: true,
                    cancel: false,
                    close: true
                }
            }
        });

        pickr.on('init', () => {
            el.style.background = input.value || '#ffffff';
            stylePickrButtons();
        });

        pickr.on('save', (color) => {
            const hex = color.toHEXA().toString();

            input.value = hex;
            el.style.background = hex;

            pickr.hide();
        });

        pickr.on('change', (color) => {
            el.style.background = color.toHEXA().toString();
        });

    });

});