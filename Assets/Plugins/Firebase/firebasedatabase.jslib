mergeInto(LibraryManager.library, {

    InitGame: function (userKey, secretKey, language, maxCount, objectName, callback, fallback) {
        var parsedUserKey = UTF8ToString(userKey);
        var parsedSecretKey = UTF8ToString(secretKey);
        var parsedLanguage = UTF8ToString(language);
        var parsedMaxCount = UTF8ToString(maxCount);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback); 
    
        try {
            
            firebase.database().ref('USERS/' + parsedUserKey).set(parsedGameScore).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedGameScore + " was posted to " + parsedUserKey);
                
                if (window.parent) {
                    const message = {
                         "messageType": "Init",
                         "userKey": parsedUserKey,
                         "secretKey": parsedSecretKey,
                         "language": parsedLanguage,
                         "maxCount": parsedMaxCount
                    };
                    window.parent.postMessage(message, "*");
                }
            });
            
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    NotifyMessage: function (messageType, objectName, callback, fallback) {
        var parsedMessageType = UTF8ToString(messageType);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback); 

        try {
            if (window.parent) {
                const message = { 
                    "messageType": parsedMessageType
                };
                window.parent.postMessage(message, "*");
            }
            
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedMessageType + " message sent");
            
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    
    PostToLauncher: function (userKey, promotionKey, gameScore, objectName, callback, fallback) {
        var parsedUserKey = UTF8ToString(userKey);
        var parsedPromotionKey = UTF8ToString(promotionKey);
        var parsedGameScore = UTF8ToString(gameScore);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback); 
        
         try {
        
            firebase.database().ref('USERS/' + parsedUserKey).set(parsedGameScore).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedGameScore + " was posted to " + parsedUserKey);
                
                if (window.parent) {
                    const message = {
                        "messageType": "Submit",
                        "userKey": parsedUserKey,
                        "secretKey": parsedPromotionKey,
                        "gameScore": parsedGameScore
                    };
                    window.parent.postMessage(message, "*");
                }
            });
        
         } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
         }
    },

    PostJSON: function (path, value, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref('USERS/' + parsedPath).set(parsedValue).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

        PostSET: function (path, value, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref(parsedPath).set(parsedValue).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
         ForRanking: function (objectName, callback, fallback) {

        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref('USERS').once('value').then(function (snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    GetJSON: function(path, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {
            firebase.database().ref('USERS/' + parsedPath).once('value').then(function(snapshot) 
            {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    
    GetDate: function(path, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {
            firebase.database().ref('DAU/' + parsedPath).once('value').then(function(snapshot) 
            {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
        GetVALUE: function(path, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {
            firebase.database().ref(parsedPath).once('value').then(function(snapshot) 
            {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PostDate: function (path, value, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref('DAU/' + parsedPath).set(parsedValue).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

        PostRANK: function (path, value, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref('RANK /' + parsedPath).set(parsedValue).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
     
     PostDayCount: function (path, pathValue, value, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedPathValue = UTF8ToString(pathValue);
        var parsedValue = UTF8ToString(value);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref('Date/' + parsedPath).child(parsedPathValue).set(parsedValue).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

     DeleteJSON: function(path, objectName, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            firebase.database().ref(parsedPath).remove().then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted");
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }

});