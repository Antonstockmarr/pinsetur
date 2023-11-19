import { TableItem } from "bootstrap-vue-next";
import { TableFieldObject } from "bootstrap-vue-next/dist/src/types";

export type RowType = {
    value: unknown;
    index: number;
    item: TableItem;
    field: TableFieldObject<Record<string, unknown>>;
    items: TableItem[];
    toggleDetails: () => void;
    detailsShowing: boolean | undefined;
}
