interface SqlInputProps {
    value: string;
    onChange: (value: string) => void;
    onGenerate: () => void;
}

export default function SqlInput({value, onChange, onGenerate}: SqlInputProps) {
    return (
        <div className="space-y-4">
    <textarea
        className="w-full h-64 p-4 border rounded-lg font-mono text-sm 
        bg-white dark:bg-gray-800
        text-gray-900 dark:text-gray-100
        border-gray-200 dark:border-gray-700
        focus:outline-none focus:ring-2 focus:ring-blue-500/20
        placeholder:text-gray-500 dark:placeholder:text-gray-400"
        placeholder="在此粘贴您的 CREATE TABLE 语句..."
        value={value}
        onChange={(e) => onChange(e.target.value)}
    />

            <button
                className="px-4 py-2 bg-black hover:bg-gray-500 text-white rounded-lg 
        transition-colors duration-200"
                onClick={() => onGenerate()}
            >
                生成关系图
            </button>
        </div>
    );
};