'use client';

import {useState, useCallback} from "react";
import {
    Node,
    Edge,
    NodeChange,
    EdgeChange,
    applyNodeChanges,
    applyEdgeChanges
} from "reactflow";
import {parseSqlToGraph} from "@/lib/sql-parser";
import {
    ErDiagram,
    ExportButton,
    ExportWordButton,
    SqlInput
} from './components';


const EXAMPLE_SQL = `
    CREATE TABLE users
    (
        id            INT PRIMARY KEY,
        username      VARCHAR(50)         NOT NULL,
        email         VARCHAR(255) UNIQUE NOT NULL,
        password_hash VARCHAR(255)        NOT NULL,
        created_at    TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        updated_at    TIMESTAMP
    );`;

export default function SqlToDiagramPage() {
    const [sqlInput, setSqlInput] = useState(EXAMPLE_SQL.trim());
    const [nodes, setNodes] = useState<Node[]>([]);
    const [edges, setEdges] = useState<Edge[]>([]);

    const handleGenerate = useCallback(() => {
        if (!sqlInput.trim()) return;

        const {nodes: newNodes, edges: newEdges} = parseSqlToGraph(sqlInput);
        setNodes(newNodes);
        setEdges(newEdges);
    }, [sqlInput]);

    const onNodesChange = useCallback((changes: NodeChange[]) => {
        setNodes((nds) => applyNodeChanges(changes, nds));
    }, []);

    const onEdgesChange = useCallback((changes: EdgeChange[]) => {
        setEdges((eds) => applyEdgeChanges(changes, eds));
    }, []);

    return (
        <div className="min-h-screen p-8 bg-background">
            <div className="max-w-7xl mx-auto space-y-8">
                <div className="flex justify-between items-center">
                    <div className="text-center space-y-2">
                        <h1 className="text-4xl font-bold text-foreground">
                            SQL 转 ER 图工具
                        </h1>
                        <p className="text-foreground/60">
                            粘贴您的建表 SQL 语句，自动生成数据库关系图
                        </p>
                    </div>
                </div>

                <SqlInput
                    value={sqlInput}
                    onChange={setSqlInput}
                    onGenerate={handleGenerate}
                />

                {nodes.length > 0 && (
                    <>
                        <div className="flex justify-end gap-2">
                            <ExportButton fileName={new Date().getTime().toString()} elementId="er-diagram-container"/>
                            <ExportWordButton fileName={new Date().getTime().toString()} nodes={nodes}/>
                        </div>
                        <div className="mt-4">
                            <ErDiagram
                                nodes={nodes}
                                edges={edges}
                                onNodesChange={onNodesChange}
                                onEdgesChange={onEdgesChange}></ErDiagram>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}
