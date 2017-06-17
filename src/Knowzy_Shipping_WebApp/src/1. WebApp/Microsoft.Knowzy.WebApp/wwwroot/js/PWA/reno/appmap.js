﻿window.AppMap = {
    id: 'reno',
    debug: true,
    logo: '.logo',
    abilities: {
        appBar: {
            keepDefaultCommands: true,
            commands: [
                {
                    id: 'PinCommand',
                    text: document.title
                },
                {
                    id: 'ShareCommand',
                    options: {
                        text: 'This is awesome! ' + document.location.href
                    }
                }
            ]
        }
    }
};