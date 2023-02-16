function InfinityScroll({ scrollElementSelector, selector, template, url, method, data, useWindowScrool = false, loaderTemplate = '', onNotFound }) {

    const selectedItem = document.querySelectorAll(selector)[0];
    const scrollElement = useWindowScrool ? window : document.querySelectorAll(scrollElementSelector)[0];

    let startDate = null;//selectedItem.dataset.startDate; 
    let endDate = null; //selectedItem.dataset.endDate;

    let isFirstLoad = true;
    let hasNext = true;
    let scrollLock = false;
    let isLoading = false;

    let xhrRequest = null;

    const addElement = (elem) => {
        selectedItem.innerHTML += (template(elem));
    };

    const addElements = (dataarray) => {
        let data = $.isArray(dataarray) ? dataarray : [dataarray];

        for (let i = 0; i < data.length; i++) {
            let elem = data[i];
            addElement(elem)
        }
    };
    const removeLoader = () => {
        selectedItem.innerHTML = selectedItem.innerHTML.replace(loaderTemplate, '');
    }

    const onSuccess = (res) => {
        startDate = res.startDate;
        endDate = res.endDate;

        removeLoader();

        if (isFirstLoad && (!res.data || !res.data.length)) {
            onNotFound();
            return;
        }

        addElements(res.data);

        isFirstLoad = false;
        hasNext = res.hasNext;
        isLoading = false;
    };

    const onError = (xhr, ajaxOptions, thrownError) => {

        removeLoader();

        console.error('Server returned with unsuccessfull response');
        isLoading = false;
    };

    const requestNext = () => {
        if (!hasNext) {
            return;
        }

        if (xhrRequest && isLoading) {
            xhrRequest.abort();
        }

        var d = data();
        if (startDate)
            d.set("StartDate", startDate);
        if (endDate)
            d.set("EndDate", endDate);

        isLoading = true;
        selectedItem.innerHTML += loaderTemplate;


        xhrRequest = $.ajax({
            url: url,
            type: method,
            data: d,
            processData: false,
            contentType: false,
            success: onSuccess,
            error: onError
        });
    };

    const start = () => {
        requestNext();
    }

    const reStart = () => {
        startDate = null;
        endDate = null;
        hasNext = true;
        isFirstLoad = true;

        selectedItem.innerHTML = '';
        requestNext();
    }

    scrollElement.addEventListener("scroll", function (e) {

        let reachedBottom = (scrollElement.scrollHeight - scrollElement.offsetHeight) == scrollElement.scrollTop;
        if (useWindowScrool) {
            reachedBottom = (scrollElement.scrollY + scrollElement.innerHeight) >= document.documentElement.scrollHeight
        }

        e.stopPropagation();
        if (reachedBottom && hasNext && !scrollLock && !isLoading) {
            scrollLock = true;
            requestNext();
            scrollLock = false;
        }
    }, false);

    InfinityScroll.prototype.start = start;
    InfinityScroll.prototype.reStart = reStart;
    return this;
}
