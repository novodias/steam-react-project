class Inventory {

    /**
     * Since there isn't a official interface for getting the user's inventories,
     * This class differs a little bit from a interface
     *
     */

    languages = Object.freeze({
        'pt': 'brazilian',
        'en': 'english'
    })

    getUrl(steamid, appid, language, count = 25) {
        if (!language) {
            language = Inventory.languages.en
        }

        return `https://steamcommunity.com/inventory/${steamid}/${appid}/2?l=${language}&count=${count}`
    }
}

module.exports = Inventory