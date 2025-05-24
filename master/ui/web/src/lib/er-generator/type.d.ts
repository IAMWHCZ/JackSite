export interface ERNode {
    id: string;
    type: 'table';
    data: {
        label: string;
        columns: {
            name: string;
            type: string;
            isPrimary: boolean;
            isForeign: boolean;
        }[];
    };
    position: { x: number; y: number };
}

export interface EREdge {
    id: string;
    source: string;
    target: string;
    sourceHandle?: string;
    targetHandle?: string;
    type: 'foreignKey';
    data: {
        sourceColumns: string[];
        targetColumns: string[];
    };
}

export interface ERDiagram {
    nodes: ERNode[];
    edges: EREdge[];
}
