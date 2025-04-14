using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Fsm.WebApp.Helper
{
    public interface INotificacionServiceHelper
    {
        ITempDataDictionary ToastNotification(string Title, string Message, NotificationPosition Position, NotificationType IconType, int Timer);

        ITempDataDictionary CustomNotification(string msj, NotificationType type, NotificationPosition position, string title, bool showConfirmButton, int timer, bool toast);
    }

    //Clase personalizada la cual genera una notificacion en sweet alert 2
    //Utiliza un temdata con las variables de sesion para enviarlas

    //Solo se debe hacer la inyeccion de depencias de la interface INotificacionServiceHelper
    public class NotificacionServiceHelper : INotificacionServiceHelper
    {
        private readonly HttpContext _httpContext;
        private readonly ITempDataDictionary _tempData;

        public NotificacionServiceHelper(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _tempData = tempDataDictionaryFactory.GetTempData(_httpContext);
        }

        public void BasicNotification(string msj, NotificationType type, string title = "")
        {
            _tempData["notification"] = $"Swal.fire('{title}','{msj}', '{type.ToString().ToLower()}')";
        }

        // el parametro de timer con valor 0 se deshabilita

        public ITempDataDictionary CustomNotification(string msj, NotificationType type, NotificationPosition position, string title = "", bool showConfirmButton = false, int timer = 2000, bool toast = true)
        {
            string pos = position.ToString().ToLower();
            string swalType = GetSwalType(type);

            _tempData["notification"] = $"Swal.fire({{ customClass: {{ confirmButton: 'btn btn-primary', cancelButton: 'btn btn-danger' }}," +
                $" position: '{pos}', icon: '{swalType}', title: '{title}', text: '{msj}', showConfirmButton: {showConfirmButton.ToString().ToLower()}" +
                $", confirmButtonColor: '#4F0DA2', toast: {toast.ToString().ToLower()}, timer: {timer} }});";

            return _tempData;
        }

        private static string GetSwalType(NotificationType type)
        {
            return type switch
            {
                NotificationType.Success => "success",
                NotificationType.Error => "error",
                NotificationType.Info => "info",
                NotificationType.Warning => "warning",
                NotificationType.Question => "question",
                _ => throw new ArgumentException("Tipo de notificación no válido.", nameof(type)),
            };
        }

        //Recibe 3 parametros 1. El mensaje 
        public ITempDataDictionary ToastNotification(string Title, string Message, NotificationPosition Position, NotificationType IconType, int timer = 2000)
        {
            string Type = SetIconType(IconType.ToString());
            string Pos = SetPosition(Position.ToString());

            _tempData["notification"] = "Swal.fire({" +
            "toast: true," +
                    "position: '" + Pos + "'," +
                    "icon: '" + Type + "'," +
                    "title: '" + Title + "'," +
                    "text: '" + Message + "'," +
                    "color: 'white'," +
                    "background: '#292b2c'," +
                    "showConfirmButton: false," +
                    "timer: '" + timer + "'," +
                    "timerProgressBar: true" +
                "})";

            return _tempData;
        }


        #region Methods

        private static string SetPosition(string position)
        {
            if (position == "Top") position = "top";
            if (position == "TopStart") position = "top-start";
            if (position == "TopEnd") position = "top-end";
            if (position == "Center") position = "center";
            if (position == "CenterStart") position = "center-start";
            if (position == "CenterEnd") position = "center-end";
            if (position == "Bottom") position = "bottom";
            if (position == "BottomStart") position = "bottom-start";
            if (position == "BottomEnd") position = "bottom-end";

            return position;
        }

        private string SetIconType(string Type)
        {
            if (Type == "Success") Type = "success";
            if (Type == "Error") Type = "error";
            if (Type == "Info") Type = "info";
            if (Type == "Warning") Type = "warning";
            if (Type == "Question") Type = "question";


            return Type;
        }

        #endregion
    }

    public enum NotificationType
    {
        Success,
        Error,
        Info,
        Warning,
        Question
    }

    public enum NotificationPosition
    {
        Top,
        TopStart,
        TopEnd,
        Center,
        CenterStart,
        CenterEnd,
        Bottom,
        BottomStart,
        BottomEnd
    }
}
