function canvasRender(timeStamp) {
    if (!window.canvasContext) {
        return;
    }

    var currentTime = Date.now();
    if (currentTime - window.canvasContext.lastUpdateTime < window.canvasContext.updateRate) {
        window.requestAnimationFrame(canvasRender);
        return;
    }
    window.canvasContext.lastUpdateTime = currentTime;

    window.canvasContext.instance.invokeMethodAsync('CanvasRender', timeStamp);
    window.requestAnimationFrame(canvasRender);
}

function canvasResize() {
    if (window.canvasContext.canvas) {
        window.canvasContext.canvas.width = window.canvasContext.viewport.clientWidth;
        window.canvasContext.canvas.height = window.canvasContext.viewport.clientHeight;

        window.canvasContext.instance.invokeMethodAsync('CanvasResize',
            window.canvasContext.canvas.width,
            window.canvasContext.canvas.height);
    }
}

function canvasPointerMove(event) {
    window.canvasContext.instance.invokeMethodAsync('CanvasPointerMove',
        event.clientX,
        event.clientY);
}

function canvasPointerDown(event) {
    window.canvasContext.instance.invokeMethodAsync('CanvasPointerDown',
        event.button,
        event.shiftKey,
        event.altKey);
}

function canvasPointerUp(event) {
    window.canvasContext.instance.invokeMethodAsync('CanvasPointerUp',
        event.button,
        event.shiftKey,
        event.altKey);
}

window.canvasInit = (instance) => {
    var viewport = document.getElementById('canvasViewport');
    var canvases = viewport.getElementsByTagName('canvas') || [];

    window.canvasContext = {
        instance: instance,
        viewport: viewport,
        canvas: canvases.length ? canvases[0] : null,
        disposed: false,
        updateRate: 1000 / 30,
        lastUpdateTime: 0
    };

    window.addEventListener("resize", canvasResize);
    window.canvasContext.viewport.addEventListener("pointermove", canvasPointerMove);
    window.canvasContext.viewport.addEventListener("pointerdown", canvasPointerDown);
    window.canvasContext.viewport.addEventListener("pointerup", canvasPointerUp);

    canvasResize();

    window.requestAnimationFrame(canvasRender);
};

window.canvasDispose = (instance) => {
    window.removeEventListener("resize", canvasResize);
    if (window.canvasContext.viewport) {
        window.canvasContext.viewport.removeEventListener("pointermove", canvasPointerMove);
        window.canvasContext.viewport.removeEventListener("pointerdown", canvasPointerDown);
        window.canvasContext.viewport.removeEventListener("pointerup", canvasPointerUp);
    }
    window.canvasContext = null;
};