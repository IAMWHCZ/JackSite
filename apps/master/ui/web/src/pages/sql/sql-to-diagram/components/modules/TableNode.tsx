import {Handle, Position} from 'reactflow';

interface Column {
    name: string;
    type: string;
    isPrimary: boolean;
    isForeign: boolean;
    references?: string;
}

interface TableNodeProps {
    data: {
        label: string;
        columns: Column[];
    };
}

export default function TableNode({data}: TableNodeProps) {
    return <div
        className="bg-background rounded-xl border border-gray-200 dark:border-gray-800 p-4 shadow-lg min-w-[250px] transition-shadow hover:shadow-xl">
        <Handle
            type="target"
            position={Position.Top}
            className="!bg-blue-500 !w-3 !h-3"
        />
        <Handle
            type="source"
            position={Position.Bottom}
            className="!bg-blue-500 !w-3 !h-3"
        />

        <div className="font-bold text-lg border-b border-gray-200 dark:border-gray-800 pb-3 mb-3 text-foreground">
            {data.label}
        </div>

        <div className="space-y-2 text-sm">
            {data.columns.map((col, index) => (
                <div
                    key={`${col.name}-${index}`}
                    className={`flex items-center gap-2 p-2 rounded ${
                        col.isPrimary ? 'bg-blue-50 dark:bg-blue-900/50' :
                            col.isForeign ? 'bg-green-50 dark:bg-green-900/50' : ''
                    }`}
                >
          <span className={`font-medium ${
              col.isPrimary ? 'text-blue-600 dark:text-blue-400' :
                  col.isForeign ? 'text-green-600 dark:text-green-400' : 'text-foreground'
          }`}>
            {col.name}
          </span>
                    <span className="text-gray-500 dark:text-gray-400">
            ({col.type})
          </span>
                    {col.isPrimary && (
                        <span
                            className="text-xs bg-blue-100 dark:bg-blue-900 text-blue-600 dark:text-blue-400 px-2 py-0.5 rounded-full">
              PK
            </span>
                    )}
                    {col.isForeign && (
                        <span
                            className="text-xs bg-green-100 dark:bg-green-900 text-green-600 dark:text-green-400 px-2 py-0.5 rounded-full">
              FK → {col.references}
            </span>
                    )}
                </div>
            ))}
        </div>
    </div>
};