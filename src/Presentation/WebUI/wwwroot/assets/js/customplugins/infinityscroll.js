function InfinityScroll({ scrollElementSelector, selector, template, url, method, data, useWindowScrool = false }) {

    const selectedItem = document.querySelectorAll(selector)[0];
    const scrollElement = useWindowScrool ? window : document.querySelectorAll(scrollElementSelector)[0];

    let startDate = undefined;//selectedItem.dataset.startDate; 
    let endDate = undefined; //selectedItem.dataset.endDate;

    let hasNext = true;
    let scrollLock = false;
    let isLoading = false;

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

    const onSuccess = (res) => {
        startDate = res.startDate;
        endDate = res.endDate;

        addElements(res.data);
        hasNext = res.hasNext;
        isLoading = false;
    };

    const onError = (xhr, ajaxOptions, thrownError) => {
        console.error('Server returned with unsuccessfull response');
        isLoading = false;
    };

    const requestNext = () => {
        if (!hasNext) {
            return;
        }
        data.set("StartDate", startDate);
        data.set("EndDate", endDate);

        isLoading = true;

        $.ajax({
            url: url,
            type: method,
            data: data,
            processData: false,
            contentType: false,
            success: onSuccess,
            error: onError
        });
    };

    const start = () => {
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
    return this;
}
