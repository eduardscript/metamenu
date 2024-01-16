type InputProps = {
    id?: string;
    placeholder: string;
    label: string,
    inputType?: React.HTMLInputTypeAttribute; 
    onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

export const Input: React.FC<InputProps> = ({
    id,
    placeholder,
    label,
    inputType = "text",
    onChange = undefined,
}) => {
    return (
        <>
            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor={id ?? label.toLowerCase()}>
                {label}
            </label>
            <input onChange={onChange} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" id={id} type={inputType} placeholder={placeholder} />
        </>);
}