window.preloadImages = function (imageNames) {

    var imagesArray = Array.from(imageNames);
    imagesArray.forEach(imageName => {
        var imagePath = 'https://cdn.pranksterapp.com/img/' + imageName;
        if (loaded[imagePath])
            return;

        link = document.createElement('link');
        link.rel = 'preload';
        link.href = imagePath;
        link.as = 'image';
        loaded[imagePath] = true;


        document.getElementsByTagName('head')[0].appendChild(link);
    });
}

loaded = [];