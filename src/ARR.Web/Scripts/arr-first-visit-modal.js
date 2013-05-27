/* Helper function that we use that will trigger showing a modal dialog only the first 
   time the user navigates to the page that hooks into this method in its onready
   function 

   Usage: 
        $(document).ready(triggerModalOnFirstVisit('arr_first_edit', '#modalIntro'));
*/
function triggerModalOnFirstVisit(cookieName, modalName) {
    if ($.cookie(cookieName) == null) {
        $(modalName).modal('show');
        $.cookie(cookieName, true, { expires: 9999, path: "/" });
    }
}