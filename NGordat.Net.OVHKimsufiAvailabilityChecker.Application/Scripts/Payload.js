/**
 * ServerStatus domain class.
 */
var ServerStatus = function () {
    this.Name = "";
    this.GeographicZone = "";
    this.Processor = "";
    this.ProcessorDetails = "";
    this.Memory = "";
    this.Storage = "";
    this.Network = "";
    this.Price = "";
    this.IsAvailable = null ;
}

/**
 * Cleans a string from any parasitial caracter.
 * @param {any} input The input string.
 */
function cleanString(input)
{
    var output = input.replace(/(\r\n|\n|\r)/gm, '');
    output = output.trim();
    output = output.replace(/\s\s/gm, '');
    return output;
}

/**
 * Launches the process of parsing and interpreting the page.
 */
function parsePageData() {
    var serverStatusArray = [];
    jQuery('table').each(function () {
        var currentServerLocation = "";

        // Step 1: Retrieve the servers location.
        var headers = jQuery(this).find('tbody > tr > th');
        if (headers.length >= 10)
        {
            var locationSpan = jQuery(headers[9]).find('span');
            if (locationSpan != null)
            {
                currentServerLocation = locationSpan.attr('class')
            }
        }

        var servers = jQuery(this).find('tbody > tr');
        servers.each(function () {

            var currentServerStatus = new ServerStatus();
            currentServerStatus.GeographicZone = currentServerLocation;

            var server = jQuery(this);
            var serverInfos = server.find('td');

            var columnCounter = 0;
            if (serverInfos.length > 0) {

                serverInfos.each(function () {
                    switch (columnCounter) {
                        case 0:
                            currentServerStatus.Name = cleanString(jQuery(this).text());
                            break;
                        case 1:
                            currentServerStatus.Processor = cleanString(jQuery(this).text());
                            break;
                        case 2:
                            currentServerStatus.ProcessorDetails = cleanString(jQuery(this).text());
                            break;
                        case 3:
                            currentServerStatus.ProcessorDetails += ' ' + cleanString(jQuery(this).text());
                            break;
                        case 4:
                            currentServerStatus.Memory = cleanString(jQuery(this).text());
                            break;
                        case 5:
                            currentServerStatus.Storage = cleanString(jQuery(this).text());
                            break;
                        case 6:
                            currentServerStatus.Network = cleanString(jQuery(this).text());
                            break;
                        case 7:
                            // IPv6
                            break;
                        case 8:
                            currentServerStatus.Price = cleanString(jQuery(this).text());
                            break;
                        case 9:
                            currentServerStatus.IsAvailable = (jQuery(this).html() !== '');
                            break;
                        default:
                            // Do nothing.
                            break;
                    }
                    columnCounter++;
                });

                serverStatusArray.push(currentServerStatus);
            }
            else
            {
                // Not a proper input.
            }
        });
    });
    console.log(serverStatusArray.length + ' elements found');

    // Send results to dotnetcallback.
    dotnetcallback.setServerStatus(JSON.stringify(serverStatusArray));
}

// Let the page correctly load.
setTimeout(parsePageData, 3000);