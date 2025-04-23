import {
    Document,
    Paragraph,
    Table,
    TableRow,
    TableCell,
    BorderStyle,
    WidthType,
    Packer,
    ShadingType,
    AlignmentType
} from 'docx';
import {saveAs} from 'file-saver';
import {Node} from 'reactflow';
import {toast} from 'react-hot-toast';

interface Column {
    name: string;
    type: string;
    isPrimary: boolean;
    isForeign: boolean;
    isUnique: boolean;
    isNullable: boolean;
    defaultValue: string | null;
    comment: string | null;
    references?: string;
}

interface TableData {
    label: string;
    columns: Column[];
}

interface ExtendedNode extends Node {
    data: TableData;
}

interface ExportWordButtonProps {
    nodes: ExtendedNode[];
    fileName?: string;
}

export default function ExportWordButton({nodes, fileName = 'diagram'}: ExportWordButtonProps) {
    const handleExport = async () => {
        try {
            const doc = new Document({
                sections: [{
                    children: nodes.map((node) => {
                        const {columns} = node.data;
                        // 创建表格标题
                        const titleParagraph = new Paragraph({
                            text: node.data.label,
                            spacing: {after: 200},
                            heading: 'Heading1',
                            alignment: AlignmentType.CENTER
                        });

                        // 创建表格
                        const table = new Table({
                            width: {
                                size: 100,
                                type: WidthType.PERCENTAGE,
                            },
                            borders: {
                                top: {style: BorderStyle.SINGLE, size: 15},
                                bottom: {style: BorderStyle.SINGLE, size: 15},
                                left: {style: BorderStyle.NONE},
                                right: {style: BorderStyle.NONE},
                                insideHorizontal: {style: BorderStyle.SINGLE, size: 15},
                                insideVertical: {style: BorderStyle.NONE},
                            },
                            rows: [
                                // 表头行
                                new TableRow({
                                    tableHeader: true,
                                    children: [
                                        new TableCell({
                                            children: [new Paragraph({text: '列名', alignment: AlignmentType.CENTER})],
                                            width: {size: 20, type: WidthType.PERCENTAGE},
                                            shading: {type: ShadingType.CLEAR, fill: 'D3D3D3'},
                                        }),
                                        new TableCell({
                                            children: [new Paragraph({
                                                text: '数据类型',
                                                alignment: AlignmentType.CENTER
                                            })],
                                            width: {size: 15, type: WidthType.PERCENTAGE},
                                            shading: {type: ShadingType.CLEAR, fill: 'D3D3D3'},
                                        }),
                                        new TableCell({
                                            children: [new Paragraph({text: '约束', alignment: AlignmentType.CENTER})],
                                            width: {size: 20, type: WidthType.PERCENTAGE},
                                            shading: {type: ShadingType.CLEAR, fill: 'D3D3D3'},
                                        }),
                                        new TableCell({
                                            children: [new Paragraph({text: '可空', alignment: AlignmentType.CENTER})],
                                            width: {size: 10, type: WidthType.PERCENTAGE},
                                            shading: {type: ShadingType.CLEAR, fill: 'D3D3D3'},
                                        }),
                                        new TableCell({
                                            children: [new Paragraph({
                                                text: '默认值',
                                                alignment: AlignmentType.CENTER
                                            })],
                                            width: {size: 15, type: WidthType.PERCENTAGE},
                                            shading: {type: ShadingType.CLEAR, fill: 'D3D3D3'},
                                        }),
                                        new TableCell({
                                            children: [new Paragraph({text: '备注', alignment: AlignmentType.CENTER})],
                                            width: {size: 20, type: WidthType.PERCENTAGE},
                                            shading: {type: ShadingType.CLEAR, fill: 'D3D3D3'},
                                        }),
                                    ],
                                }),
                                // 数据行
                                ...columns.map((col: Column) => {
                                    const constraints = [];
                                    if (col.isPrimary) constraints.push('PRIMARY KEY');
                                    if (col.isForeign) constraints.push(`FOREIGN KEY → ${col.references}`);
                                    if (col.isUnique) constraints.push('UNIQUE');

                                    return new TableRow({
                                        children: [
                                            new TableCell({
                                                children: [new Paragraph({
                                                    text: col.name,
                                                    alignment: AlignmentType.CENTER
                                                })],
                                                width: {size: 20, type: WidthType.PERCENTAGE},
                                            }),
                                            new TableCell({
                                                children: [new Paragraph({
                                                    text: col.type,
                                                    alignment: AlignmentType.CENTER
                                                })],
                                                width: {size: 15, type: WidthType.PERCENTAGE},
                                            }),
                                            new TableCell({
                                                children: [new Paragraph({
                                                    text: constraints.join(', '),
                                                    alignment: AlignmentType.CENTER
                                                })],
                                                width: {size: 20, type: WidthType.PERCENTAGE},
                                            }),
                                            new TableCell({
                                                children: [new Paragraph({
                                                    text: col.isNullable ? 'YES' : 'NO',
                                                    alignment: AlignmentType.CENTER
                                                })],
                                                width: {size: 10, type: WidthType.PERCENTAGE},
                                            }),
                                            new TableCell({
                                                children: [new Paragraph({
                                                    text: col.defaultValue ?? '-',
                                                    alignment: AlignmentType.CENTER
                                                })],
                                                width: {size: 15, type: WidthType.PERCENTAGE},
                                            }),
                                            new TableCell({
                                                children: [new Paragraph({
                                                    text: col.comment ?? '-',
                                                    alignment: AlignmentType.CENTER
                                                })],
                                                width: {size: 20, type: WidthType.PERCENTAGE},
                                            }),
                                        ],
                                    });
                                })
                            ],
                        });

                        return [
                            titleParagraph,
                            table,
                            new Paragraph({text: '', spacing: {after: 500}})
                        ];
                    }).flat()
                }]
            });

            // 使用 Packer 来生成文档
            const blob = await Packer.toBlob(doc);
            saveAs(blob, `${fileName}.docx`);
        } catch (error: unknown) {
            console.error('Export failed:', error);
            toast.error('导出失败');
        }
    };

    const onExportClick = () => {
        void handleExport();
    };

    return (
        <div className="relative group">
            <button
                onClick={onExportClick}
                className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg shadow-sm hover:shadow-md transition-all duration-200 flex items-center gap-2"
                aria-label="导出表结构为 Word"
            >
                <svg
                    className="w-5 h-5"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                    />
                </svg>
                导出表结构文档
            </button>
            <div
                className="absolute bottom-full left-1/2 -translate-x-1/2 mb-2 px-3 py-2 bg-gray-900 text-white text-sm rounded-lg opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-all duration-200 whitespace-nowrap">
                将所有表结构以三线表格式导出为 Word 文档，包含完整的字段信息
            </div>
        </div>
    );
}
