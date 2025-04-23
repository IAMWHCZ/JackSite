import ReactFlow, {
    Background,
    Controls,
    Node,
    Edge,
    ReactFlowInstance,
    NodeChange,
    EdgeChange
} from 'reactflow';
import 'reactflow/dist/style.css';
import TableNode from "@/pages/sql/sql-to-diagram/components/modules/TableNode.tsx";
import {ExtendedWindow} from "@/pages/sql/sql-to-diagram/type";


const nodeTypes = {
    tableNode: TableNode,
};

interface ErDiagramProps {
    nodes: Node[];
    edges: Edge[];
    onNodesChange: (changes: NodeChange[]) => void;
    onEdgesChange: (changes: EdgeChange[]) => void;
}

declare const window: ExtendedWindow;

export default function ErDiagram({nodes, edges, onNodesChange, onEdgesChange}: ErDiagramProps) {
    const onInit = (flowInstance: ReactFlowInstance) => {
        window.flowInstance = flowInstance;
    };

    return (
        <div
            id="er-diagram-container"
            className="border border-gray-200 dark:border-gray-800 rounded-xl bg-background"
            style={{height: '70vh'}}
        >
            <ReactFlow
                nodes={nodes}
                edges={edges}
                onNodesChange={onNodesChange}
                onEdgesChange={onEdgesChange}
                nodeTypes={nodeTypes}
                onInit={onInit}
                fitView
                defaultViewport={{x: 0, y: 0, zoom: 1}}
                minZoom={0.1}
                maxZoom={4}
                attributionPosition="bottom-left"
                proOptions={{hideAttribution: true}}
                className="rounded-xl bg-background"
            >
                <Background
                    color="#94a3b8"
                    gap={24}
                    size={1.5}
                    className="!bg-background"
                />
                <Controls
                    className="!bg-background !shadow-lg !rounded-xl !border !border-gray-200 dark:!border-gray-800"
                    showInteractive={false}
                />
            </ReactFlow>
        </div>
    );
}
