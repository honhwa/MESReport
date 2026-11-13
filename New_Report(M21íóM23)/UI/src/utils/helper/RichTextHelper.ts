export function ReplaceSpace(e:string) {
    return e = e.replace(/&nbsp;/g, "\xa0"),
           e = e.replace(/&quot;/g, "'"),
           e = e.replace(/'/g, "''"),  
           e = e.replace(/&amp;/g, "&"), 
    e = e.replace(/&lt;/g, "<"), e = e.replace(/&gt;/g, ">");
}