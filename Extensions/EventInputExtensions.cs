namespace Common
{
    public static class EventInputExtensions
    {
        public static EventInput GetEventInput(this EventInput[] eventInputs, EventCode type)
        {
            for (int i = 0; i < eventInputs.Length; i++)
            {
                if (eventInputs[i].Type == type)
                {
                    return eventInputs[i];
                }
            }
            return null;
        }

        public static EventInput GetEventInput(this IInput[] inputs, EventCode type)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                var eventInput = inputs[i] as EventInput;
                if (eventInput != null && eventInput.Type == type)
                {
                    return eventInput;
                }
            }
            return null;
        }

        public static EventInput GetEventInput(this UserInputData[] userInputDatas, EventCode type, out int index)
        {
            index = -1;
            for (int i = 0; i < userInputDatas.Length; i++)
            {
                var eventInput = GetEventInput(userInputDatas[i].Inputs, type);
                if (eventInput != null)
                {
                    index = i;
                    return eventInput;
                }
            }
            return null;
        }
    }
}
