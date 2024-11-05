export function observeElement(elementId, dotNetObject, threshold = [0.2, 1]) {
    const target = document.getElementById(elementId);
    if (!target) return;

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {

            dotNetObject.invokeMethodAsync('OnIntersectionChanged', entry.isIntersecting);
        });
    }, {
        root: null, // viewport
        threshold: threshold // % of visibility to trigger
    });

    observer.observe(target);
}