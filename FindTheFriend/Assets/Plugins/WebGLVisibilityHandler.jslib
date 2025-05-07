mergeInto(LibraryManager.library, {
    SetupVisibilityListener: function () {
        document.addEventListener("visibilitychange", function() {
            if (document.hidden) {
                // Страница скрыта (переключили вкладку или свернули браузер)
                unityInstance.SendMessage('AudioManager', 'OnPageHidden');
            } else {
                // Страница снова активна
                unityInstance.SendMessage('AudioManager', 'OnPageVisible');
            }
        });
    }
});