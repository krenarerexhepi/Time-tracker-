using System;

namespace TimeTrackerMobile{

public class Model {

    private String name;
    private Boolean selected;

    public Model(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

		public Boolean isSelected() {
        return selected;
    }

		public void setSelected(Boolean selected) {
        this.selected = selected;
    }
}
}