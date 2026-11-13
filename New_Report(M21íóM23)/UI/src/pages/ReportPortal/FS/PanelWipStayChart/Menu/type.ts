export type IMenuList = {
    label: string,
    icon?: string,
    children?: IMenuList[],
    onClick: Function
}