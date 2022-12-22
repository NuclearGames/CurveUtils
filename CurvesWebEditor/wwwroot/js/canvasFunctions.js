function canvasRender(timeStamp) {
    window.canvasContext.instance.invokeMethodAsync('CanvasRender', timeStamp);
    window.requestAnimationFrame(canvasRender);
}

function canvasResize() {
    if (window.canvasContext.canvas) {
        window.canvasContext.canvas.width = window.canvasContext.holder.clientWidth;
        window.canvasContext.canvas.height = window.canvasContext.holder.clientHeight;

        window.canvasContext.instance.invokeMethodAsync('CanvasResize',
            window.canvasContext.canvas.width,
            window.canvasContext.canvas.height);
    }
}

window.canvasInit = (instance) => {
    var holder = document.getElementById('canvasHolder');
    var canvases = holder.getElementsByTagName('canvas') || [];

    window.canvasContext = {
        instance: instance,
        holder: holder,
        canvas: canvases.length ? canvases[0] : null
    };

    window.addEventListener("resize", canvasResize);
    canvasResize();

    window.requestAnimationFrame(canvasRender);
};